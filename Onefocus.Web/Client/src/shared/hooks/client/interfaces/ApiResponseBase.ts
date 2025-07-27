import Error from "./Error";

export default interface ApiResponseBase {
    status: number;
    title: string;
    type: string;
    errors?: Error[];
}