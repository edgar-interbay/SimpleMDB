import { $, apiFetch, renderStatus, getQueryParam, captureMovieForm } from '/scripts/common.js';

(async function initMovieEdit() {
  const id       = getQueryParam('id');
  const form     = $('#movie-form');
  const statusEl = $('#status');

  if (!id) {
    renderStatus(statusEl, 'err', 'Missing ?id in URL.');
    form.querySelectorAll('input,textarea,button,select').forEach(el => el.disabled = true);
    return;
  }

  try {
    const m = await apiFetch(`/movies/${encodeURIComponent(id)}`);
    form.title.value       = m.title ?? '';
    form.year.value        = m.year ?? '';
    form.description.value = m.description ?? '';
    renderStatus(statusEl, 'ok', 'Loaded movie. You can edit and save.');
  } catch (err) {
    renderStatus(statusEl, 'err', `Failed to load data: ${err.message}`);
    return;
  }

  form.addEventListener('submit', async (ev) => {
    ev.preventDefault();
    const payload = captureMovieForm(form);
    try {
      const updated = await apiFetch(`/movies/${encodeURIComponent(id)}`, {
        method: 'PUT',
        body: JSON.stringify(payload),
      });
      renderStatus(statusEl, 'ok',
        `Updated movie #${updated.id} "${updated.title}" (${updated.year}).`);
    } catch (err) {
      renderStatus(statusEl, 'err', `Update failed: ${err.message}`);
    }
  });
})();
