import { $, apiFetch, renderStatus } from '/scripts/common.js';

(async function initActorAdd() {
  const form     = $('#actor-form');
  const statusEl = $('#status');
  renderStatus(statusEl, 'ok', 'New actor. Fill in the details and save.');

  form.addEventListener('submit', async (ev) => {
    ev.preventDefault();
    const payload = {
      firstName: form.firstName.value.trim(),
      lastName:  form.lastName.value.trim(),
      rating:    Number(form.rating.value)
    };
    try {
      const created = await apiFetch('/actors', { method: 'POST', body: JSON.stringify(payload) });
      renderStatus(statusEl, 'ok', `Created actor #${created.id} "${created.firstName} ${created.lastName}".`);
      form.reset();
    } catch (err) {
      renderStatus(statusEl, 'err', `Create failed: ${err.message}`);
    }
  });
})();
