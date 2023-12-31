interface UserPayslip {
  firstName: string;
  lastName: string;
  cardNumber: string;
  contractType: string;
  location: string;
  position: string;
  totalSalaryAndBenefits: string;
  totalDeductions: string;
  netPayable: string;
  year: number;
  month: string;
  fileId: string;

  salaryAndBenefits?: { [key: number]: string } | null;
  durations?: { [key: number]: string } | null;
  salaryAndBenefitsAmount?: { [key: number]: string } | null;
  deductions?: { [key: number]: string } | null;
  deductionsAmount?: { [key: number]: string } | null;
}
