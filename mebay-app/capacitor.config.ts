import { CapacitorConfig } from '@capacitor/cli';

const config: CapacitorConfig = {
  appId: 'mebay.app',
  appName: 'mebay-app',
  webDir: 'www',
  server: {
    androidScheme: 'https'
  }
};

export default config;
