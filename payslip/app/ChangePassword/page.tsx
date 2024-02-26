import Profile from "../components/Profile";

const ChangePassword = () => {
  return (
    <div className="">
      <h1 className="text-5xl text-center mt-10 mb-28">
        لطفا رمز عبور خود را تغییر دهید.
      </h1>
      <Profile shouldSignOut={true} />
    </div>
  );
};

export default ChangePassword;
