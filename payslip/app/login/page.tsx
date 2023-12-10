import LoginForm from "./form";
import { FaUser } from "react-icons/fa";

const Login = () => {
  return (
    <div className="bg-gradient-to-b from-gray-800 via-cyan-900 to-cyan-950 h-screen flex items-center justify-center">
      <div className=" bg-slate-950 font-bold max-w-lg shadow-lg shadow-slate-900 rounded-lg text-gray-300 px-20 py-10">
        <div className="flex justify-center">
          <span className="border rounded-full p-6 text-6xl">
            <FaUser />
          </span>
        </div>
        <h1 className="text-3xl text-center pt-4 pb-3">ورود کاربران</h1>

        <LoginForm />
      </div>
    </div>
  );
};

export default Login;
