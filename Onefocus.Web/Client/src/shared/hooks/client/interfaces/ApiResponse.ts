import ApiResponseBase from "./ApiResponseBase";

export default interface ApiResponse<T = void>  extends ApiResponseBase {
    value: T;
}