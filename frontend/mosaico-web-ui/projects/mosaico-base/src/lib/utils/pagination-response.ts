export interface PaginationResponse<T> {
    total: number;
    entities: T[];
}