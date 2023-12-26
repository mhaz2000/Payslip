import { getServerSession } from "next-auth";
import { redirect } from "next/navigation";
import AdminTabs from "./components/AdminTabs";
export default async function DashboardLayout({
  children,
}: {
  children: React.ReactNode;
}) {
  const session: any = await getServerSession();
  if (session && session?.user["name"] !== "admin") redirect("/403");
  return (
    <main>
      <AdminTabs />
      {children}
    </main>
  );
}
