"use client";

import "react-toastify/ReactToastify.css";

import useAxiosAuth from "@/lib/hooks/useAxiosAuth";
import { FormEvent, useState } from "react";
import { displayError, displaySuccess } from "@/lib/toastDisplay";

interface CreateUserFormProps {
  closeModal: () => void;
  handleCreateUser: () => void;
}

const CreateUserForm: React.FC<CreateUserFormProps> = ({
  closeModal,
  handleCreateUser,
}) => {
  const axiosAuth = useAxiosAuth();

  const [firstNameEmpty, setFirstNameEmpty] = useState(false);
  const [lastNameEmpty, setLastNameEmpty] = useState(false);
  const [nationalCodeEmpty, setNationalCodeEmpty] = useState(false);
  const [cardNumberEmpty, setCardNumberEmpty] = useState(false);

  const createUserRequest = async (
    firstName: string,
    lastName: string,
    nationalCode: string,
    cardNumber: string
  ) => {
    try {
      const res = await axiosAuth.post(`/api/Users`, {
        firstName: firstName,
        lastName: lastName,
        nationalCode: nationalCode,
        cardNumber: cardNumber,
      });

      if (res.status === 200) {
        displaySuccess("کاربر جدید با موفقیت ایجاد شد.");
        closeModal();
        handleCreateUser();
      }
    } catch (error: any) {
      displayError(error.data.message);
    }
  };

  const handleSubmit = async (e: FormEvent<HTMLFormElement>) => {
    e.preventDefault();

    const formData = new FormData(e.currentTarget);

    const firstName = formData.get("firstName");
    const lastName = formData.get("lastName");
    const nationalCode = formData.get("nationalCode");
    const cardNumber = formData.get("cardNumber");

    if (
      firstName &&
      lastName &&
      nationalCode &&
      cardNumber &&
      (firstName.valueOf() as string).length &&
      (lastName.valueOf() as string).length &&
      (nationalCode.valueOf() as string).length &&
      (cardNumber.valueOf() as string).length
    ) {
      const res = await createUserRequest(
        firstName.valueOf() as string,
        lastName.valueOf() as string,
        nationalCode.valueOf() as string,
        cardNumber.valueOf() as string
      );
    } else {
      if (!firstName || (firstName.valueOf() as string).trim().length === 0)
        setFirstNameEmpty(true);

      if (!lastName || (lastName.valueOf() as string).trim().length === 0)
        setLastNameEmpty(true);

      if (
        !nationalCode ||
        (nationalCode.valueOf() as string).trim().length === 0
      )
        setNationalCodeEmpty(true);

      if (!cardNumber || (cardNumber.valueOf() as string).trim().length === 0)
        setCardNumberEmpty(true);
    }
  };
  return (
    <>
      <div className="relative p-6 flex-auto">
        <form
          className="flex flex-col gap-2 sm:gap-10 mx-auto text-sm px-5 relative"
          onSubmit={handleSubmit}
        >
          <div className="flex flex-col sm:flex-row gap-4 w-full">
            <div className="flex flex-col w-full gap-2">
              <label className="text-right text-gray-300">نام</label>
              <input
                name="firstName"
                type="text"
                onChange={(e) => {
                  if (e.target.value.length > 0) setFirstNameEmpty(false);
                }}
                className={`border border-black text-black rounded-md h-8 w-full p-2 mx-auto
          ${firstNameEmpty ? "bg-red-400" : "bg-slate-200"}`}
              />
            </div>
            <div className="flex flex-col w-full gap-2">
              <label className="text-right text-gray-300">نام خانوادگی</label>
              <input
                name="lastName"
                type="text"
                onChange={(e) => {
                  if (e.target.value.length > 0) setLastNameEmpty(false);
                }}
                className={`border border-black text-black rounded-md h-8 w-full p-2 mx-auto
          ${lastNameEmpty ? "bg-red-400" : "bg-slate-200"}`}
              />
            </div>
          </div>
          <div className="flex flex-col sm:flex-row gap-4 w-full">
            <div className="flex flex-col w-full gap-2">
              <label className="text-right text-gray-300">کد ملی</label>
              <input
                name="nationalCode"
                type="number"
                onChange={(e) => {
                  if (e.target.value.length > 0) setNationalCodeEmpty(false);
                }}
                className={`border border-black text-black rounded-md h-8 w-full p-2 mx-auto
          ${nationalCodeEmpty ? "bg-red-400" : "bg-slate-200"}`}
              />
            </div>

            <div className="flex flex-row w-full gap-2">
              <div className="flex flex-col w-full gap-2">
                <label className="text-right text-gray-300">شماره کارت</label>
                <input
                  name="cardNumber"
                  type="number"
                  onChange={(e) => {
                    if (e.target.value.length > 0) setCardNumberEmpty(false);
                  }}
                  className={`border border-black text-black rounded-md h-8 w-full p-2 mx-auto
          ${cardNumberEmpty ? "bg-red-400" : "bg-slate-200"}`}
                />
              </div>
            </div>
          </div>
          <div className="flex items-center justify-end p-6 border-t border-solid border-blueGray-200 rounded-b gap-4">
            <div className="mx-auto flex flex-row gap-4">
              <button
                className="btn-style bg-transparent border-red-500 !text-red-500 hover:bg-red-500 w-24 hover:!text-black"
                type="submit"
                onClick={() => closeModal()}
              >
                بستن
              </button>
              <button
                className="btn-style bg-transparent border-green-500 !text-green-500 hover:bg-green-500 w-24 hover:!text-black"
                type="submit"
              >
                ثبت
              </button>
            </div>
          </div>
        </form>
      </div>
    </>
  );
};

export default CreateUserForm;
