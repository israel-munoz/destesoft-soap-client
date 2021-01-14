# SOAP Client for .NET

## Project website
https://www.destesoft.com/soap-client/

## Making SOAP requests easy!
The SOAP messaging protocol was extensively used in the past. We have new (and better) options today, like REST. However, many services we may require will still use the SOAP protocol.

**SOAP Client for .NET** allows us to make requests to SOAP services in a much simpler and cleaner way than using the old *Web Service References* from early versions of Visual Studio.

SOAP Client is configured at runtime, executes the method we ask at the moment and allows us to map the result directly to a native type or custom class, depending on the response we are expecting.

### Installation
You can install the package from the Visual Studio [Package Management](https://docs.microsoft.com/en-us/nuget/consume-packages/install-use-packages-visual-studio) window or using the Install-Package command:

```shell
Install-Package DesteSoft.SoapClient
```

### How to use it
**SOAP Client** consists in only two steps: creating the client instance and making the request.

```C#
// Create instance of client
SoapClient client = new SoapClient("[SOAP service URL]", "[SOAP service namespace]");

// Make a void request
client.Post("[SOAP action]", [optional parameters]);

// Make a request and map result to a variable
string result = client.Post<string>("[SOAP action]", [parameters]);
```

#### SOAP service URL
The URL to the service we'll make requests to. If it's a ASP.NET Web Service, it should be the path to the ASMX file.

#### SOAP service namespace
The namespace can be found in the SOAP service WSDL as the targetNamespace attribute of the root node.

#### SOAP action
The specific *web method* we are requesting.

#### parameters (optional)
The parameters required by the SOAP action. They can be found in the service's WSDL, in the corresponding element node inside types.

There are different ways to make the request, all of them use the same parameters.

```C#
// Sync request returning no value
client.Post("SOAP action");

// Async request returning no value
await client.PostAsync("SOAP action");

// Sync request returning a value
int value = client.Post<int>("SOAP action");

// Async request returning a value
CustomClass result = await client.PostAsync<CustomClass>("SOAP action");
```

### Mapping to custom classes
You may need to refer to the SOAP service's WSDL to check the type returned from a SOAP action.

The type sent to the generic method, Post<T> or PostAsync<T>, should match the SOAP action response type. If not matched, the default value will be set instead.

```XML
<definitions ...>
  <types>
    <schema ...>
      ...
      <complexType name="Item">
        <sequence>
          <element minOccurs="1" maxOccurs="1" name="Id" type="int" />
          <element minOccurs="1" maxOccurs="1" name="Name" type="string" />
          <element minOccurs="1" maxOccurs="1" name="CreatedDate" type="dateTime" />
        </sequence>
      </complexType>
      <element name="SoapActionResponse">
        <complexType>
          <sequence>
            <element minOccurs="0" maxOccurs="1" name="SoapActionResult" type="Item" />
          </sequence>
        </complexType>
      </element>
      ...
    </schema>
  </types>
  ...
</definitions>
```

In this example, the mapping class should look like this:

```C#
public class ResponseItem
{
  public int Id { get; set; }
  public string Name { get; set; }
  public DateTime Id { get; set; }
}
```

Not all of the properties need to match in the class. Unmatched values will get a default value.
