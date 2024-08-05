/** @type {import('next').NextConfig} */
const nextConfig = {
  images: {
    // images.domains got deprecated
    // domains: ["cdn.pixabay.com"],
    remotePatterns: [
      {
        protocol: "https",
        hostname: "cdn.pixabay.com",
      },
    ],
  },
  logging: {
    fetches: {
      fullUrl: true,
    },
  },
};

export default nextConfig;
