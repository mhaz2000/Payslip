import { useSession } from "next-auth/react";
import { useEffect } from "react";
import { axiosAuth } from "../axios";
import { useRouter } from "next/navigation";

const useAxiosAuth = () => {
  const { data: session } = useSession();
  const router = useRouter();

  useEffect(() => {
    const requestIntercept = axiosAuth.interceptors.request.use((config) => {
      if (!config.headers["Authorization"])
        config.headers["Authorization"] = `Bearer ${session?.user.authToken}`;

      return config;
    });

    const responseIntercept = axiosAuth.interceptors.response.use(
      (response) => response,
      (error) => {
        debugger
        // Handle errors here
        if(error.response.status === 401 )
          router.push('/login')
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
