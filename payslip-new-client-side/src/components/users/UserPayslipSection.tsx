import { useEffect, useRef, useState } from "react";
import logo from "/logo.png";
import { userPayslip } from "../../api/payslip";
import type { UserPayslip } from "../../types/payslip";
import { MonthMapper } from "../../utils/monthMapper";
import { useTranslation } from "react-i18next";
import { useReactToPrint } from "react-to-print";

interface TableRow {
  salaryAndBenefitTitle: string;
  salaryAndBenefitAmount: string;
  salaryAndBenefitDuration: string;
  deductionTitle: string;
  deductionAmount: string;
  descriptionTitle: string;
  descriptionAmount: string;
}

interface Props {
  year: number | null;
  month: number | null;
}

export default function UserPayslipSection({ year, month }: Props) {
  const { t } = useTranslation();

  const [payslip, setPayslip] = useState<UserPayslip | null>(null);
  const [rows, setRows] = useState<TableRow[]>([]);

  const printRef = useRef<HTMLDivElement>(null);

  useEffect(() => {
    const fetchPayslip = async () => {
      if (!month || !year) return;

      try {
        const data = await userPayslip(month, year);
        setPayslip(data);

        const rowCount = Math.max(
          Object.keys(data.salaryAndBenefits ?? {}).length,
          Object.keys(data.deductions ?? {}).length
        );

        const mappedRows: TableRow[] = Array.from({ length: rowCount }, (_, i) => {
          const index = i + 1;
          return {
            salaryAndBenefitTitle: data.salaryAndBenefits?.[index] ?? "",
            salaryAndBenefitAmount: data.salaryAndBenefitsAmount?.[index] ?? "",
            salaryAndBenefitDuration: data.durations?.[index] ?? "",
            deductionTitle: data.deductions?.[index] ?? "",
            deductionAmount: data.deductionsAmount?.[index] ?? "",
            descriptionTitle: data.descriptions?.[index] ?? "",
            descriptionAmount: data.descriptionsAmount?.[index] ?? "",
          };
        });

        setRows(mappedRows);
      } catch (error) {
        console.error(t("fetchError"), error);
      }
    };

    fetchPayslip();
  }, [month, year, t]);

  const handlePrint = useReactToPrint({
    contentRef: printRef,
    documentTitle: `${t("payslip")}_${year}_${month}`,
    pageStyle: `
      @page {
        size: auto;
        margin: 20mm;
      }
      body {
        direction: rtl;
      }
    `,
  });

  return (
    <div className="text-black">
      {payslip && (
        <>
          {/* Content to print */}
          <div ref={printRef}>
            <div className="p-6 rounded-lg bg-white">
              <PayslipTable payslip={payslip} rows={rows} year={year!} month={month!} />
            </div>
          </div>

          {/* Print button */}
          <div className="mt-4 no-print">
            <button
              onClick={handlePrint}
              className="px-4 py-2 bg-blue-600 text-white rounded hover:bg-blue-700"
            >
              {t("print")}
            </button>
          </div>
        </>
      )}
    </div>
  );
}

function PayslipTable({
  payslip,
  rows,
  year,
  month,
}: {
  payslip: UserPayslip;
  rows: TableRow[];
  year: number;
  month: number;
}) {
  const { t,i18n } = useTranslation();

  const isRtl = i18n.dir() === 'rtl';

  return (
    <div className="text-sm text-black">
      <div className="flex justify-between items-center mb-6">
        <img src={logo} alt="Logo" className="w-28 h-auto" />
        <div className="text-center">
          <h2 className="text-lg font-bold">{t("companyName")}</h2>
          <h3 className="text-sm mt-2">
            {t("title", {
              month: t(MonthMapper.get(month)!),
              year,
            })}
          </h3>
        </div>
        <div className="w-28" />
      </div>

      <div className="grid grid-cols-3 gap-4 text-sm mb-4">
        <div>
          <p>
            <strong>{t("firstName")}:</strong> {payslip.firstName}
          </p>
          <p>
            <strong>{t("contractType")}:</strong> {payslip.contractType}
          </p>
        </div>
        <div>
          <p>
            <strong>{t("lastName")}:</strong> {payslip.lastName}
          </p>
          <p>
            <strong>{t("location")}:</strong> {payslip.location}
          </p>
        </div>
        <div>
          <p>
            <strong>{t("cardNumber")}:</strong> {payslip.cardNumber}
          </p>
          <p>
            <strong>{t("position")}:</strong> {payslip.position}
          </p>
        </div>
      </div>

      <div className="overflow-x-auto">
        <table className="min-w-full border border-black text-xs">
          <thead className="bg-gray-300 text-right font-medium">
            <tr>
              <th className="border border-black px-2 py-1">{t("salaryAndBenefits")}</th>
              <th className="border border-black px-2 py-1">{t("duration")}</th>
              <th className="border border-black px-2 py-1">{t("amount")}</th>
              <th className="border border-black px-2 py-1">{t("deductions")}</th>
              <th className="border border-black px-2 py-1">{t("amount")}</th>
              <th className="border border-black px-2 py-1" colSpan={2}>
                {t("descriptions")}
              </th>
            </tr>
          </thead>
          <tbody>
            {rows.map((row, i) => (
              <tr key={i} className={i % 2 === 0 ? "bg-white" : "bg-gray-100"}>
                <td className="border border-black px-2 py-1">{row.salaryAndBenefitTitle}</td>
                <td dir={isRtl ? 'ltr': 'rtl'} className="border border-black px-2 py-1 text-left">{row.salaryAndBenefitDuration}</td>
                <td className="border border-black px-2 py-1">{row.salaryAndBenefitAmount}</td>
                <td className="border border-black px-2 py-1">{row.deductionTitle}</td>
                <td className="border border-black px-2 py-1">{row.deductionAmount}</td>
                <td className="border border-black px-2 py-1">{row.descriptionTitle}</td>
                <td className="border border-black px-2 py-1">{row.descriptionAmount}</td>
              </tr>
            ))}
          </tbody>
          <tfoot className="font-bold bg-gray-200">
            <tr>
              <td colSpan={2} className="border border-black px-2 py-1">
                {t("totalSalaryAndBenefits")}
              </td>
              <td className="border border-black px-2 py-1">{payslip.totalSalaryAndBenefits}</td>
              <td className="border border-black px-2 py-1">{t("totalDeductions")}</td>
              <td className="border border-black px-2 py-1">{payslip.totalDeductions}</td>
              <td className="border border-black px-2 py-1">{t("netPayable")}</td>
              <td className="border border-black px-2 py-1">{payslip.netPayable}</td>
            </tr>
          </tfoot>
        </table>
      </div>
    </div>
  );
}
