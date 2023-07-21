export class PaginatedList<T> {
  count: number = 0;
  currentPage: number = 0;
  totalPages: number = 0;
  pageSize: number = 0;
  items: T[] = [];
}
