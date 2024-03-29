import type { Metadata } from "next";
import "./globals.css";
import Provider from "./Provider";
import { getServerSession } from "next-auth/next";
import Navbar from "./components/Navbar";
import { ToastContainer } from "react-toastify";

export const metadata: Metadata = {
  title: "فیش حقوقی",
  description: "Generated by create next app",
};

export default async function RootLayout({
  children,
}: {
  children: React.ReactNode;
}) {
  const session = await getServerSession();
  

  // console.log(session);
  return (
    <html lang="en">
      <body className="font-IranSansFaNumbers gradient-bg h-screen dir-right">
        <ToastContainer
          rtl
          limit={3}
          autoClose={3000}
          closeOnClick={true}
          draggable={false}
          pauseOnHover={true}
          style={{ width: "400px" }}
        />
        <Provider>
          {session && <Navbar />}
          {children}
        </Provider>
      </body>
    </html>
  );
}
