import { $, apiFetch, renderStatus, clearChildren, getQueryParam } from '/scripts/common.js';

(async function initMovieActors() {
  const movieId  = getQueryParam('id');
  const statusEl = $('#status');

  if (!movieId) return renderStatus(statusEl, 'err', 'Missing ?id in URL.');

  // Load movie title
  try {
    const movie = await apiFetch(`/movies/${movieId}`);
    $('#movie-title').textContent = `${movie.title} (${movie.year})`;
    $('#view-link').href = `/movies/view.html?id=${movieId}`;
  } catch (err) {
    return renderStatus(statusEl, 'err', `Failed to load movie: ${err.message}`);
  }

  // Populate actor dropdown
  const select = $('#actor-select');
  try {
    const allActors = await apiFetch('/actors');
    for (const a of allActors) {
      const opt = document.createElement('option');
      opt.value       = a.id;
      opt.textContent = `${a.firstName} ${a.lastName}`;
      select.appendChild(opt);
    }
  } catch (err) {
    renderStatus(statusEl, 'err', `Failed to load actors: ${err.message}`);
  }

  async function loadLinkedActors() {
    const listEl = $('#actor-list');
    const tpl    = $('#actor-card');
    clearChildren(listEl);
    try {
      const links = await apiFetch(`/movies/${movieId}/actors`);
      if (links.length === 0) {
        listEl.innerHTML = '<p class="meta">No actors linked to this movie yet.</p>';
        return;
      }
      for (const link of links) {
        const actor = await apiFetch(`/actors/${link.actorId}`).catch(() => null);
        if (!actor) continue;
        const frag = tpl.content.cloneNode(true);
        const root = frag.querySelector('.card');
        root.querySelector('.fullname').textContent      = `${actor.firstName} ${actor.lastName}`;
        root.querySelector('.btn-unlink').dataset.actorId = actor.id;
        listEl.appendChild(frag);
      }

      listEl.addEventListener('click', async (ev) => {
        const btn = ev.target.closest('button.btn-unlink[data-actor-id]');
        if (!btn) return;
        const actorId = btn.dataset.actorId;
        if (!confirm('Remove this actor from the movie?')) return;
        try {
          await apiFetch(`/movies/${movieId}/actors/${actorId}`, { method: 'DELETE' });
          renderStatus(statusEl, 'ok', 'Actor removed from movie.');
          await loadLinkedActors();
        } catch (err) {
          renderStatus(statusEl, 'err', `Remove failed: ${err.message}`);
        }
      }, { once: true });

      renderStatus(statusEl, '', '');
    } catch (err) {
      renderStatus(statusEl, 'err', `Failed to load linked actors: ${err.message}`);
    }
  }

  await loadLinkedActors();

  // Add actor form
  $('#add-actor-form').addEventListener('submit', async (ev) => {
    ev.preventDefault();
    const actorId = select.value;
    try {
      await apiFetch(`/movies/${movieId}/actors/${actorId}`, { method: 'POST' });
      renderStatus(statusEl, 'ok', 'Actor added to movie.');
      await loadLinkedActors();
    } catch (err) {
      renderStatus(statusEl, 'err', `Add failed: ${err.message}`);
    }
  });
})();
