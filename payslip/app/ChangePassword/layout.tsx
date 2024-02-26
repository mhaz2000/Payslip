"use client";
import { useRouter } from "next/navigation";
import { useSession } from "next-auth/react";

export default function ChangePasswordLayout({
  children,
}: {
  children: React.ReactNode;
}) {
  const { data: session } = useSession();
  const router = useRouter();
  if (!session) router.push("/login");

  return <main>{children}</main>;
}
