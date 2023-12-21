import type { Metadata } from "next";
import "./globals.css";
import Provider from "./Provider";
import { getServerSession } from "next-auth/next";
import Navbar from "./components/Navbar";

export const metadata: Metadata = {
  title: "Create Next App",
  description: "Generated by create next app",
};

export default async function RootLayout({ children }: { children: React.ReactNode }) {
  const session = await getServerSession()

  return (
    <html lang="en">
      <body className="font-IranSans gradient-bg h-screen dir-right">
        <Provider>
          {session && <Navbar />}
          {children}
          </Provider>
      </body>
    </html>
  );
}