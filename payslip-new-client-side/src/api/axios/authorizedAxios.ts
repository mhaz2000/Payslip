import axios from "axios";
import i18n from "../../i18n";

const persianDigits = ["۰", "۱", "۲", "۳", "۴", "۵", "۶", "۷", "۸", "۹"];

function toPersianDigits(input: string): string {
  return input.replace(/\d/g, (d) => persianDigits[+d]);
}

const englishDigits = ["0", "1", "2", "3", "4", "5", "6", "7", "8", "9"];

function toEnglishDigits(input: string): string {
  return input.replace(/[۰-۹]/g, (d) => englishDigits[persianDigits.indexOf(d)]);
}

function convertObjectNumbersToEnglishDigits(obj: any): any {
  if (typeof obj === "string") {
    return toEnglishDigits(obj);
  } else if (Array.isArray(obj)) {
    return obj.map(convertObjectNumbersToEnglishDigits);
  } else if (obj !== null && typeof obj === "object") {
    const newObj: any = {};
    for (const key in obj) {
      newObj[key] = convertObjectNumbersToEnglishDigits(obj[key]);
    }
    return newObj;
  }
  return obj;
}

function convertObjectNumbersToPersianDigits(obj: any): any {
  if (typeof obj === "string") {
    return toPersianDigits(obj);
  } else if (Array.isArray(obj)) {
    return obj.map(convertObjectNumbersToPersianDigits);
  } else if (obj !== null && typeof obj === "object") {
    const newObj: any = {};
    for (const key in obj) {
      newObj[key] = convertObjectNumbersToPersianDigits(obj[key]);
    }
    return newObj;
  }
  return obj;
}

const authorizedAxios = axios.create({
  baseURL: import.meta.env.VITE_API_BASE_URL + "api/",
});

authorizedAxios.interceptors.request.use((config) => {
  const token = localStorage.getItem("token");
  if (token) {
    config.headers.Authorization = `Bearer ${token}`;
  }

  // Convert Persian digits in request data to English digits
  if (i18n.language === "fa" && config.data && !(config.data instanceof FormData)) {
    config.data = convertObjectNumbersToEnglishDigits(config.data);
  }
  if (i18n.language === "fa" && config.url) {
    config.url = convertObjectNumbersToEnglishDigits(config.url);
  }

  return config;
});

authorizedAxios.interceptors.response.use(
  (response) => {
    // Only convert digits if current language is Persian
    if (i18n.language === "fa" && response.data) {
      response.data = convertObjectNumbersToPersianDigits(response.data);
    }
    return response;
  },
  (error) => {
    if (error.response?.status === 401 || error.response?.status === 403) {
      localStorage.removeItem("token");

      const referrer = document.referrer;
      const redirectUrl = referrer.includes("/admin") ? "/admin/login" : "/login";

      window.location.href = redirectUrl;
    }

    return Promise.reject(error);
  }
);

export default authorizedAxios;
