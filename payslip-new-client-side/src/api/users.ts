import type { ApiResponse } from "../types/api";
import type { User, UserEntry } from "../types/user";
import { authorizedAxios } from "./axios";

export const getAllUsers = async (page: number, search: string): Promise<ApiResponse<User[]>> => {
    const { data } = await authorizedAxios.get<ApiResponse<User[]>>(`Users?skip=${(page - 1) * 7}&search=${search}`);
    return data;
};

export const toggleActivation = async (id: string): Promise<void> => {
    await authorizedAxios.put(`Users/ToggleActivation/${id}`)
}

export const createUser = async (user: UserEntry): Promise<void> => {
    await authorizedAxios.post('Users', user);
}

export const updateUser = async (user: UserEntry, userId: string): Promise<void> => {
    await authorizedAxios.put(`Users/${userId}`, user);
}

export const importUsersFromExcel = async (file: File): Promise<string> => {
  const formData = new FormData();
  formData.append("File", file);

  const response = await authorizedAxios.post<{ id: string }>("Users/UploadUsersFile", formData, {
    headers: {
      "Content-Type": "multipart/form-data",
    },
  });

  // assuming your API returns something like { id: "someId" }
  return response.data.id;
};