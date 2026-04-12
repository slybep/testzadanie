import React, { useEffect, useState } from 'react';
import type { LinkResponse } from '../src/types/link';
import { getLinks, createLink, deleteLink, updateLink } from './api/links';

function App() {
  const [links, setLinks] = useState<LinkResponse[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);
  
  // Пагинация
  const [currentPage, setCurrentPage] = useState(1);
  const [pageSize, setPageSize] = useState(5);
  
  // Форма добавления
  const [newUrl, setNewUrl] = useState('');
  const [isCreating, setIsCreating] = useState(false);
  
  // Редактирование
  const [editingId, setEditingId] = useState<string | null>(null);
  const [editUrl, setEditUrl] = useState('');

  const loadLinks = async () => {
    setLoading(true);
    setError(null);
    try {
      const data = await getLinks();
      setLinks(data);
    } catch (err) {
      setError(err instanceof Error ? err.message : 'Unknown error');
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    loadLinks();
  }, []);

  const handleCreate = async (e: React.FormEvent) => {
    e.preventDefault();
    if (!newUrl.trim()) return;
    setIsCreating(true);
    try {
      await createLink({ url: newUrl });
      setNewUrl('');
      await loadLinks();
      // Если после добавления текущая страница не содержит новых элементов, можно остаться на ней.
    } catch (err) {
      alert('Ошибка создания');
    } finally {
      setIsCreating(false);
    }
  };

  const handleDelete = async (id: string) => {
    if (!confirm('Удалить ссылку?')) return;
    try {
      await deleteLink(id);
      await loadLinks();
      // Если после удаления текущая страница оказалась пустой, переключаем на предыдущую
      const totalAfterDelete = links.length - 1;
      const maxPage = Math.ceil(totalAfterDelete / pageSize);
      if (currentPage > maxPage && maxPage > 0) setCurrentPage(maxPage);
    } catch (err) {
      alert('Ошибка удаления');
    }
  };

  const handleUpdate = async (id: string) => {
    if (!editUrl.trim()) return;
    try {
      await updateLink(id, editUrl);
      setEditingId(null);
      setEditUrl('');
      await loadLinks();
    } catch (err) {
      alert('Ошибка обновления');
    }
  };

  // Пагинация на клиенте
  const totalPages = Math.ceil(links.length / pageSize);
  const paginatedLinks = links.slice((currentPage - 1) * pageSize, currentPage * pageSize);

  const goToPage = (page: number) => {
    setCurrentPage(Math.min(Math.max(1, page), totalPages));
  };
  const BACKEND_BASE = 'http://localhost:5196'
  
  const getShortUrl = (shortUrl: string) => {
    return `${BACKEND_BASE}/Link/${shortUrl}`;
  };

  return (
    <div className="container mx-auto p-4 max-w-4xl">
      <h1 className="text-3xl font-bold mb-6">Сокращатель ссылок</h1>

      {/* Форма добавления */}
      <form onSubmit={handleCreate} className="mb-8 p-4 bg-gray-100 rounded-lg flex gap-2">
        <input
          type="url"
          value={newUrl}
          onChange={(e) => setNewUrl(e.target.value)}
          placeholder="Введите длинную ссылку (http://...)"
          required
          className="flex-1 p-2 border rounded"
        />
        <button
          type="submit"
          disabled={isCreating}
          className="px-4 py-2 bg-indigo-600 text-white rounded hover:bg-indigo-700 disabled:opacity-50"
        >
          {isCreating ? 'Создание...' : 'Сократить'}
        </button>
      </form>

      {/* Список ссылок */}
      {loading && <div className="text-center py-10">Загрузка...</div>}
      {error && <div className="text-center py-10 text-red-600">Ошибка: {error}</div>}
      {!loading && !error && (
        <>
          <div className="overflow-x-auto">
            <table className="min-w-full bg-white border rounded-lg">
              <thead className="bg-gray-200">
                <tr>
                  <th className="px-4 py-2 border">Оригинальный URL</th>
                  <th className="px-4 py-2 border">Короткая ссылка</th>
                  <th className="px-4 py-2 border">Кликов</th>
                  <th className="px-4 py-2 border">Действия</th>
                </tr>
              </thead>
              <tbody>
                {paginatedLinks.map((link) => (
                  <tr key={link.id} className="hover:bg-gray-50">
                    <td className="px-4 py-2 border break-all">
                      {editingId === link.id ? (
                        <input
                          type="url"
                          value={editUrl}
                          onChange={(e) => setEditUrl(e.target.value)}
                          className="w-full p-1 border rounded"
                          autoFocus
                        />
                      ) : (
                        <a href={link.url} target="_blank" rel="noopener noreferrer" className="text-blue-600 hover:underline">
                          {link.url.length > 60 ? link.url.slice(0, 60) + '…' : link.url}
                        </a>
                      )}
                    </td>
                    <td className="px-4 py-2 border">
                      <a href={getShortUrl(link.shortUrl)} target="_blank" rel="noopener noreferrer" className="text-green-600 hover:underline">
                        {getShortUrl(link.shortUrl)}
                      </a>
                    </td>
                    <td className="px-4 py-2 border text-center">{link.countClick}</td>
                    <td className="px-4 py-2 border whitespace-nowrap">
                      {editingId === link.id ? (
                        <>
                          <button
                            onClick={() => handleUpdate(link.id)}
                            className="px-2 py-1 bg-green-500 text-white rounded text-sm mr-1 hover:bg-green-600"
                          >
                            Сохранить
                          </button>
                          <button
                            onClick={() => setEditingId(null)}
                            className="px-2 py-1 bg-gray-500 text-white rounded text-sm hover:bg-gray-600"
                          >
                            Отмена
                          </button>
                        </>
                      ) : (
                        <>
                          <button
                            onClick={() => {
                              setEditingId(link.id);
                              setEditUrl(link.url);
                            }}
                            className="px-2 py-1 bg-yellow-500 text-white rounded text-sm mr-1 hover:bg-yellow-600"
                          >
                            Изменить
                          </button>
                          <button
                            onClick={() => handleDelete(link.id)}
                            className="px-2 py-1 bg-red-500 text-white rounded text-sm hover:bg-red-600"
                          >
                            Удалить
                          </button>
                        </>
                      )}
                    </td>
                  </tr>
                ))}
                {paginatedLinks.length === 0 && (
                  <tr>
                    <td colSpan={4} className="text-center py-6 text-gray-500">
                      Нет ссылок. Создайте первую!
                    </td>
                  </tr>
                )}
              </tbody>
            </table>
          </div>

          {/* Пагинация */}
          {totalPages > 1 && (
            <div className="flex justify-between items-center mt-6">
              <div className="flex gap-2">
                <button
                  onClick={() => goToPage(currentPage - 1)}
                  disabled={currentPage === 1}
                  className="px-3 py-1 border rounded disabled:opacity-50"
                >
                  Назад
                </button>
                <span className="px-3 py-1">
                  Страница {currentPage} из {totalPages}
                </span>
                <button
                  onClick={() => goToPage(currentPage + 1)}
                  disabled={currentPage === totalPages}
                  className="px-3 py-1 border rounded disabled:opacity-50"
                >
                  Вперёд
                </button>
              </div>
              <div className="flex items-center gap-2">
                <label className="text-sm">Показывать:</label>
                <select
                  value={pageSize}
                  onChange={(e) => {
                    setPageSize(Number(e.target.value));
                    setCurrentPage(1);
                  }}
                  className="border rounded p-1"
                >
                  {[5, 10, 20, 50].map(size => (
                    <option key={size} value={size}>{size}</option>
                  ))}
                </select>
              </div>
            </div>
          )}
        </>
      )}
    </div>
  );
}

export default App;