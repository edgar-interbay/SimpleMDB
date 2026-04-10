import { $, apiFetch, renderStatus, getQueryParam } from '/scripts/common.js';

(async function initActorEdit() {
  const id       = getQueryParam('id');
  const form     = $('#actor-form');
  const statusEl = $('#status');

  if (!id) {
    renderStatus(statusEl, 'err', 'Missing ?id in URL.');
    form.querySelectorAll('input,button').forEach(el => el.disabled = true);
    return;
  }

  try {
    const a = await apiFetch(`/actors/${id}`);
    form.firstName.value = a.firstName ?? '';
    form.lastName.value  = a.lastName  ?? '';
    form.rating.value    = a.rating    ?? 0;
    renderStatus(statusEl, 'ok', 'Loaded actor. You can edit and save.');
  } catch (err) {
    renderStatus(statusEl, 'err', `Failed to load data: ${err.message}`);
    return;
  }

  form.addEventListener('submit', async (ev) => {
    ev.preventDefault();
    const payload = {
      firstName: form.firstName.value.trim(),
      lastName:  form.lastName.value.trim(),
      rating:    Number(form.rating.value)
    };
    try {
      const updated = await apiFetch(`/actors/${id}`, { method: 'PUT', body: JSON.stringify(payload) });
      renderStatus(statusEl, 'ok', `Updated actor #${updated.id} "${updated.firstName} ${updated.lastName}".`);
    } catch (err) {
      renderStatus(statusEl, 'err', `Update failed: ${err.message}`);
    }
  });
})();
