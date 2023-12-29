import { useSession } from "next-auth/react";
import { useEffect } from "react";
import { axiosAuth, axiosFormDataAuth } from "../axios";
import { useRouter } from "next/navigation";
import { AxiosInstance } from "axios";

const useAxiosAuth = (formData: boolean | null = null) => {
  const { data: session } = useSession();
  const router = useRouter();
  let axios: AxiosInstance;

  if (formData) axios = axiosFormDataAuth;
  else axios = axiosAuth;

  useEffect(() => {
    const requestIntercept = axios.interceptors.request.use((config) => {
      if (!config.headers["Authorization"])
        config.headers["Authorization"] = `Bearer ${session?.user.authToken}`;

      return config;
    });

    const responseIntercept = axios.interceptors.response.use(
      (response) => response,
      (error) => {
        // Handle errors here
        if (error.response.status === 401) router.push("/login");
        if (error.response) {
          return Promise.reject({
            status: error.response.status,
            data: error.response.data,
          });
        } else if (error.request) {
          // The request was made but no response was received
          console.error("No response received:", error.request);
        } else {
          // Something happened in setting up the request that triggered an Error
          console.error("Request setup error:", error.message);
        }

        // Or return a new Promise to handle the error gracefully
        return Promise.reject(error);
      }
    );

    return () => {
      axios.interceptors.request.eject(requestIntercept);
      axios.interceptors.response.eject(responseIntercept);
    };
  }, [session]);

  return axios;
};

export default useAxiosAuth;
