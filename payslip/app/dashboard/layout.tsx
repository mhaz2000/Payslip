import DashboardTabs from "./components/DashboardTabs";

export default async function DashboardLayout({
  children,
}: {
  children: React.ReactNode;
}) {
  return (
    <main>
      <DashboardTabs />
      {children}
    </main>
  );
}
