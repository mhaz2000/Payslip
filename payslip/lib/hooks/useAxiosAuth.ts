import { useSession } from "next-auth/react";
import { useEffect } from "react";
import { axiosAuth } from "../axios";

const useAxiosAuth = () => {
  const { data: session } = useSession();

  useEffect(() => {
    const requestIntercept = axiosAuth.interceptors.request.use((config) => {
      if (!config.headers["Authorization"])
        config.headers["Authorization"] = `Bearer ${session?.user.authToken}`;

      return config;
    });

    const responseIntercept = axiosAuth.interceptors.response.use(
      (response) => response,
      (error) => {
        // Handle errors here
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
      axiosAuth.interceptors.request.eject(requestIntercept);
      axiosAuth.interceptors.response.eject(responseIntercept);
    };
  }, [session]);

  return axiosAuth;
};

export default useAxiosAuth;
