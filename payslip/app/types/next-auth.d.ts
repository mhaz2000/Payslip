import NextAuth from "next-auth";

declare module "next-auth" {
  interface Session {
    user: {
      isAdmin: boolean;
      authToken: string;
      refreshToken: string;
      expiresIn: string;
      fullName: string;
      mustChangePassword: boolean;
    };
  }
}
