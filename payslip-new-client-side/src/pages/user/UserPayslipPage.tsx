import { useEffect, useState } from "react";
import { userWages } from "../../api/payslip";
import UserPayslipSection from "../../components/users/UserPayslipSection";
import type { UserWage } from "../../types/payslip";
import { MonthMapper } from "../../utils/monthMapper";
import { useTranslation } from "react-i18next";
import { toPersianDigits } from "../../utils/persianDigits";

export default function UserPayslipPage() {
  const { t } = useTranslation();

  const [wages, setWages] = useState<UserWage[]>([]);
  const [selectedYear, setSelectedYear] = useState<number | null>(null);
  const [selectedMonth, setSelectedMonth] = useState<number | null>(null);
  const [availableMonths, setAvailableMonths] = useState<number[]>([]);

  useEffect(() => {
    const fetchWages = async () => {
      try {
        const data = await userWages();
        setWages(data);
      } catch (error) {
        console.error(t("errorFetchingWages"), error);
      }
    };

    fetchWages();
  }, [t]);

  useEffect(() => {
    if (selectedYear) {
      const yearData = wages.find((w) => w.year === selectedYear);
      setAvailableMonths(yearData?.months || []);
      setSelectedMonth(null);
    }
  }, [selectedYear, wages]);

  return (
    <div className="p-6 space-y-4">
      <h2 className="text-2xl text-primary">{t("ChooseYearAndMonth")}</h2>
      <div className="flex gap-4 items-center ">
        <div className="w-1/3">
          <label className="block mb-1 font-medium">{t("year")}</label>
          <select
            value={selectedYear ?? ""}
            onChange={(e) => setSelectedYear(Number(e.target.value))}
            className="border border-gray-300 rounded px-3 py-2 w-full"
          >
            <option value="">{t("selectYear")}</option>
            {wages.map((w) => (
              <option key={w.year} value={w.year}>
                {toPersianDigits(w.year)}
              </option>
            ))}
          </select>
        </div>

        <div className="w-1/3">
          <label className="block mb-1 font-medium">{t("month")}</label>
          <select
            value={selectedMonth ?? ""}
            onChange={(e) => setSelectedMonth(Number(e.target.value))}
            disabled={!selectedYear}
            className="border border-gray-300 rounded px-3 py-2 w-full"
          >
            <option value="">{t("selectMonth")}</option>
            {availableMonths.map((m) => (
              <option key={m} value={m}>
                {t(MonthMapper.get(m)!)}
              </option>
            ))}
          </select>
        </div>
      </div>

      {selectedMonth && selectedYear && (
        <UserPayslipSection month={selectedMonth} year={selectedYear} />
      )}
    </div>
  );
}
