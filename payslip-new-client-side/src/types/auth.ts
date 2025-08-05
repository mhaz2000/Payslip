export type LoginRequest = {
    username: string;
    password: string;
}


export type UpdatePasswordEntry = {
    oldPassword: string;
    newPassword: string;
}


export type LoginResult = {
    expiresIn:string;
    authToken:string;
    refreshToken:string;
    fullName: string;
    mustChangePassword: boolean;

}