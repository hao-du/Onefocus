import ErrorResponse from "./ErrorResponse";

export default interface ApiResponseBase {
    status: number;
    title: string;
    type: string;
    errors?: ErrorResponse[];
}