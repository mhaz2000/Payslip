'use client'

import useAxiosAuth from "@/lib/hooks/useAxiosAuth";
import { useEffect } from "react";

const UserPayslip = ({month, year }:{month:number|null, year:number|null}) => {
    const axiosAuth = useAxiosAuth();
    useEffect(() => {
        const fetchData = async () => {
            axiosAuth.get('api/payslips');
        }
    },[month, year])
    
    return ( 
        <div className=""></div>
     );
}
 
export default UserPayslip;