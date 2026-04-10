import { $, apiFetch, renderStatus, getQueryParam, clearChildren } from '/scripts/common.js';

(async function initActorView() {
  const id       = getQueryParam('id');
  const statusEl = $('#status');

  if (!id) return renderStatus(statusEl, 'err', 'Missing ?id in URL.');

  try {
    const a = await apiFetch(`/actors/${id}`);
    $('#actor-id').textContent        = a.id;
    $('#actor-firstname').textContent = a.firstName;
    $('#actor-lastname').textContent  = a.lastName;
    $('#actor-rating').textContent    = `★ ${a.rating}`;
    $('#edit-link').href = `/actors/edit.html?id=${a.id}`;
    renderStatus(statusEl, 'ok', 'Actor loaded successfully.');

    // Load movies for this actor
    const links      = await apiFetch(`/movies/${id}/actors`).catch(() => []);
    const moviesEl   = $('#actor-movies');
    clearChildren(moviesEl);
    if (links.length === 0) {
      moviesEl.innerHTML = '<p class="meta">No movies linked.</p>';
    } else {
      for (const link of links) {
        const movie = await apiFetch(`/movies/${link.movieId}`).catch(() => null);
        if (!movie) continue;
        const card = document.createElement('div');
        card.className = 'card';
        card.innerHTML = `<h3>${movie.title} <span class="badge">${movie.year}</span></h3>`;
        moviesEl.appendChild(card);
      }
    }
  } catch (err) {
    renderStatus(statusEl, 'err', `Failed to load actor ${id}: ${err.message}`);
  }
})();
