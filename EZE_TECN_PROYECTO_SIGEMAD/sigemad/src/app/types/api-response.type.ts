export type ApiResponse<T> = {
	count: number;
	page: number;
	pageSize: number;
	data: T;
	pageCount: number;
}