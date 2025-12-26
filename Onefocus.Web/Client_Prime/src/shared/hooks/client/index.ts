import client from "./client";
import ApiResponse from "./interfaces/ApiResponse";
import ApiResponseBase from "./interfaces/ApiResponseBase";
import useMutation from "./useMutation";
import useQuery from "./useQuery";
import useQueryClient from "./useQueryClient";

export {
    client, useMutation,
    useQuery, useQueryClient
};
export type {
    ApiResponse,
    ApiResponseBase
};

