const forms = document.querySelectorAll('form');

forms.forEach(f => f.addEventListener('submit', onFormSubmit));

/**
 * @param { SubmitEvent } e
 */
function onFormSubmit(e) {
  e.preventDefault();
  e.stopPropagation();

  /** @type { HTMLFormElement } */
  const form = e.target;
  const data = {};
  form.querySelectorAll('*[name]').forEach(i => {
    switch (i.type) {
      case 'checkbox':
        data[i.name] = i.checked;
        break;
      default:
        data[i.name] = i.value;
    }
  });

  request(form.action, form.method, data)
    .then(result => showFormResult(form, result));
}

/**
 * 
 * @param { HTMLFormElement } form
 * @param { any } result
 */
function showFormResult(form, result) {
  const message = form.querySelector('.result');
  message.innerHTML = result;
}

/**
 * @param { string } url
 * @param { 'get' | 'post' | 'put' | 'patch' | 'delete' } method
 * @param { any } data
 */
function request(url, method, data) {
  const req = fetch(
    url,
    {
      method,
      body: method === 'get' ? null : JSON.stringify(data),
      headers: {
        'Content-Type': 'application/json'
      }
    }
  );
  return new Promise((resolve, reject) => {
    req.then(r => r.json()).then(r => {
      resolve(r.d);
      return;
      const val = JSON.stringify(JSON.parse(r.d), (k, v) => k === '__type' ? undefined : v, '  ');
      resolve(val)
    });
  });
}
