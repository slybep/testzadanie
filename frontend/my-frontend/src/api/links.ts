import type { LinkResponse, CreateLinkRequest} from '../types/link.ts';

const BASE_URL = 'http://localhost:5196/Link';

export async function getLinks(): Promise<LinkResponse[]> {
  const res = await fetch(`${BASE_URL}/urls`);
  if (!res.ok) throw new Error('Failed to fetch links');
  return res.json();
}

export async function createLink(data: CreateLinkRequest): Promise<LinkResponse> {
  const res = await fetch(BASE_URL, {
    method: 'POST',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify(data),
  });
  if (!res.ok) throw new Error('Failed to create link');
  return res.json();
}

export async function deleteLink(id: string): Promise<void> {
  const res = await fetch(`${BASE_URL}/${id}`, { method: 'DELETE' });
  if (!res.ok) throw new Error('Failed to delete link');
}

export async function updateLink(id: string, url: string): Promise<LinkResponse> {
  const res = await fetch(`${BASE_URL}/${id}`, {
    method: 'PATCH',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify({ url }),
  });
  if (!res.ok) throw new Error('Failed to update link');
  return res.json();
}