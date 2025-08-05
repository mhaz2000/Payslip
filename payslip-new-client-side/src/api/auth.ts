import type { LoginRequest, LoginResult, UpdatePasswordEntry } from '../types/auth';
import { anonymousAxios, authorizedAxios } from './axios/index';

export const loginUser = async (credentials: LoginRequest): Promise<LoginResult> => {
    const { data } = await anonymousAxios.post<LoginResult>('Authentication/Login', credentials);
    return data;
};

export const loginAdmin = async (credentials: LoginRequest): Promise<LoginResult> => {
    const { data } = await anonymousAxios.post<LoginResult>('Authentication/Login', credentials);
    return data;
};


export const updatePassword = async (passwordEntry: UpdatePasswordEntry): Promise<void> => {
    await authorizedAxios.put<UpdatePasswordEntry>('Authentication/ChangePassword', passwordEntry);
};
