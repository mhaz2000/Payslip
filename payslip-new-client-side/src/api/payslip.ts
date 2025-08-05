import type { ApiResponse } from "../types/api";
import type { PayslipFile, UserPayslip, UserWage } from "../types/payslip";
import { authorizedAxios } from "./axios";

export const getAllPayslips = async (page: number, search: string): Promise<ApiResponse<PayslipFile[]>> => {
    const { data } = await authorizedAxios.get<ApiResponse<PayslipFile[]>>(`Payslips?skip=${(page - 1) * 7}&search=${search}`);
    return data;
};

export const removePayslip = async (fileId: string): Promise<void> => {
    await authorizedAxios.delete(`Payslips/${fileId}`);
};

export const downloadPayslip = async (id: string, month: string, year: number): Promise<void> => {
    const response = await authorizedAxios.get<Blob>(`/Files/${id}`, {
        responseType: "blob", // Important for downloading files
    });

    const blob = new Blob([response.data]);
    const url = window.URL.createObjectURL(blob);
    const link = document.createElement("a");
    link.href = url;

    // Attempt to extract filename from headers or fallback
    const contentDisposition = response.headers["content-disposition"];
    let fileName = `${month}-${year}.xlsx`;

    if (contentDisposition) {
        const match = contentDisposition.match(/filename="?([^"]+)"?/);
        if (match?.[1]) {
            fileName = match[1];
        }
    }

    link.setAttribute("download", fileName);
    document.body.appendChild(link);
    link.click();
    link.remove();
    window.URL.revokeObjectURL(url);
};

export const uploadPayslip = async (file: File, month: number, year: number): Promise<string> => {
    const formData = new FormData();
    formData.append("File", file);
    formData.append("Month", month.toString());
    formData.append("Year", year.toString());


    const response = await authorizedAxios.post<{ id: string }>("Payslips", formData, {
        headers: {
            "Content-Type": "multipart/form-data",
        },
    });

    // assuming your API returns something like { id: "someId" }
    return response.data.id;
};


export const userPayslip = async (month: number, year: number): Promise<UserPayslip> => {
    const { data } = await authorizedAxios.get<UserPayslip>(`Payslips/UserPayslip?month=${month}&year=${year}`);
    return data;
}

export const userWages = async (): Promise<UserWage[]> => {
    const { data } = await authorizedAxios.get<UserWage[]>(`Payslips/wages`);
    return data;
}