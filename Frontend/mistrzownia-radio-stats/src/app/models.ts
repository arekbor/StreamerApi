export interface Pager<T> {
  totalPages: number;
  pageNumber: number;
  pageSize: number;
  totalRecords: number;
  items: T[];
}

export interface SteamStat {
  username: string;
  avatarUrl: string;
  profileUrl: string;
  youtubeUrl: string;
  youTubeName: string;
  dateTime: Date;
}
