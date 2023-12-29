import axios from "axios";
const BASE_URL = process.env.NEXT_PUBLIC_API_URL;

export const axiosAuth = axios.create({
  baseURL: BASE_URL,
  headers: { "Content-Type": "application/json" },
});

export const axiosFormDataAuth = axios.create({
  baseURL: BASE_URL,
  headers: { "Content-Type": "multipart/form-data" },
});