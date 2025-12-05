<template>
  <q-layout view="lHh Lpr lFf">
    <LayoutHeader @toggle-left-drawer="toggleLeftDrawer" />

    <LayoutDrawer v-model="leftDrawerOpen" :items="generalMenuItems" />

    <q-page-container>
      <router-view />
    </q-page-container>
  </q-layout>
</template>

<script lang="ts">
import { defineComponent, ref } from 'vue';
import LayoutDrawer from 'components/LayoutDrawer.vue';
import LayoutHeader from 'components/LayoutHeader.vue';
import type { MenuItem } from 'src/types/menu';

const generalMenuItems: MenuItem[] = [
  { label: 'Societa', caption: 'Anagrafici', icon: 'business', link: '#societa' },
  { label: 'Utenti', caption: 'Anagrafici', icon: 'groups', link: '#utenti' },
  { label: 'Divise', caption: 'Tabelle', icon: 'currency_exchange', link: '#divise' },
  { label: 'Nazioni', caption: 'Tabelle', icon: 'public', link: '#nazioni' },
  { label: 'CAP', caption: 'Tabelle', icon: 'local_post_office', link: '#cap' },
  { label: 'Banche', caption: 'Tabelle', icon: 'account_balance', link: '#banche' },
  { label: 'SMTP / POP3', caption: 'Tabelle', icon: 'settings_applications', link: '#smtp-pop3' },
  { label: 'Alla societa', caption: 'Autorizzazioni', icon: 'home_work', link: '#alla-societa' },
  { label: 'Ai menu', caption: 'Autorizzazioni', icon: 'vpn_key', link: '#ai-menu' },
];

export default defineComponent({
  name: 'MainLayout',
  components: {
    LayoutHeader,
    LayoutDrawer,
  },

  setup() {
    const leftDrawerOpen = ref(false);

    return {
      generalMenuItems,
      leftDrawerOpen,
      toggleLeftDrawer() {
        leftDrawerOpen.value = !leftDrawerOpen.value;
      },
    };
  },
});
</script>
