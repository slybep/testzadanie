export interface LinkResponse {
  id: string;          
  shortUrl: string;
  url: string;
  countClick: number;
}

export interface CreateLinkRequest {
  url: string;
}

export interface UpdateUrlRequest {
  url: string;
}