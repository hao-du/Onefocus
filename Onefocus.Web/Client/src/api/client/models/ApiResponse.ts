export type ApiResponse<T> = {
    status: number;
    title: string;
    type: string;
    value: T;
}