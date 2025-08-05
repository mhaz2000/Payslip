export interface User {
    id: string;
    firstName: string;
    lastName: string;
    nationalCode: string;
    cardNumber: string;
    isActive: boolean;
}

export interface UserEntry {
    firstName: string;
    lastName: string;
    nationalCode: string;
    cardNumber: string;
    password: string;
}
