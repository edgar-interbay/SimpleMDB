import { $, apiFetch, renderStatus, captureMovieForm } from '/scripts/common.js';

(async function initMovieAdd() {
  const form     = $('#movie-form');
  const statusEl = $('#status');
  renderStatus(statusEl, 'ok', 'New movie. You can edit and save.');

  form.addEventListener('submit', async (ev) => {
    ev.preventDefault();
    const payload = captureMovieForm(form);
    try {
      const created = await apiFetch('/movies', { method: 'POST', body: JSON.stringify(payload) });
      renderStatus(statusEl, 'ok',
        `Created movie #${created.id} "${created.title}" (${created.year}).`);
      form.reset();
    } catch (err) {
      renderStatus(statusEl, 'err', `Create failed: ${err.message}`);
    }
  });
})();
