"use client";
import { useSession } from "next-auth/react";
import DashboardTabs from "./components/DashboardTabs";
import { useRouter } from "next/navigation";

export default async function DashboardLayout({
  children,
}: {
  children: React.ReactNode;
}) {
  const { data: session } = useSession();
  const router = useRouter();

  if (session?.user.mustChangePassword) router.push("/ChangePassword");
  return (
    <main>
      <DashboardTabs />
      {children}
    </main>
  );
}
