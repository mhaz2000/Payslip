export enum Month {
  Farvardin = 1,
  Ordibehesht,
  Khordad,
  Tir,
  Mordad,
  Shahrivar,
  Mehr,
  Aban,
  Azar,
  Dey,
  Bahman,
  Esfand,
}

export const MonthMapper = new Map<number, string>([
  [Month.Farvardin, "فروردین"],
  [Month.Ordibehesht, "اردیبهشت"],
  [Month.Khordad, "خرداد"],
  [Month.Tir, "تیر"],
  [Month.Mordad, "مرداد"],
  [Month.Shahrivar, "شهریور"],
  [Month.Mehr, "مهر"],
  [Month.Aban, "آبان"],
  [Month.Azar, "آذر"],
  [Month.Dey, "دی"],
  [Month.Bahman, "بهمن"],
  [Month.Esfand, "اسفند"],
]);
