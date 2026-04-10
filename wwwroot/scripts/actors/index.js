import { $, apiFetch, renderStatus, clearChildren } from '/scripts/common.js';

(async function initActorsIndex() {
  const listEl   = $('#actor-list');
  const statusEl = $('#status');
  const tpl      = $('#actor-card');

  try {
    const actors = await apiFetch('/actors');
    clearChildren(listEl);

    if (actors.length === 0) {
      renderStatus(statusEl, 'warn', 'No actors found.');
    } else {
      renderStatus(statusEl, '', '');
      for (const a of actors) {
        const frag = tpl.content.cloneNode(true);
        const root = frag.querySelector('.card');
        root.querySelector('.fullname').textContent  = `${a.firstName} ${a.lastName}`;
        root.querySelector('.rating').textContent    = `★ ${a.rating}`;
        root.querySelector('.btn-view').href         = `/actors/view.html?id=${a.id}`;
        root.querySelector('.btn-edit').href         = `/actors/edit.html?id=${a.id}`;
        root.querySelector('.btn-delete').dataset.id = a.id;
        listEl.appendChild(frag);
      }
    }

    listEl.addEventListener('click', async (ev) => {
      const btn = ev.target.closest('button.btn-delete[data-id]');
      if (!btn) return;
      const id = btn.dataset.id;
      if (!confirm('Delete this actor? This cannot be undone.')) return;
      try {
        await apiFetch(`/actors/${id}`, { method: 'DELETE' });
        renderStatus(statusEl, 'ok', `Actor ${id} deleted.`);
        setTimeout(() => location.reload(), 1500);
      } catch (err) {
        renderStatus(statusEl, 'err', `Delete failed: ${err.message}`);
      }
    });
  } catch (err) {
    renderStatus(statusEl, 'err', `Failed to fetch actors: ${err.message}`);
  }
})();
