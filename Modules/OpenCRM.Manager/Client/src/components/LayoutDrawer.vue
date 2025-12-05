<template>
  <q-drawer v-model="drawerOpen" show-if-above bordered>
    <q-list padding>
      <q-expansion-item
        default-open
        expand-separator
        icon="dashboard_customize"
        label="Generali"
      >
        <q-item
          v-for="item in items"
          :key="item.label"
          clickable
          v-ripple
          tag="a"
          :href="item.link"
        >
          <q-item-section avatar>
            <q-icon :name="item.icon" />
          </q-item-section>

          <q-item-section>
            <q-item-label>{{ item.label }}</q-item-label>
            <q-item-label caption>{{ item.caption }}</q-item-label>
          </q-item-section>
        </q-item>
      </q-expansion-item>
    </q-list>
  </q-drawer>
</template>

<script lang="ts">
import { computed, defineComponent, type PropType } from 'vue';
import type { MenuItem } from 'src/types/menu';

export default defineComponent({
  name: 'LayoutDrawer',
  props: {
    modelValue: {
      type: Boolean,
      required: true,
    },
    items: {
      type: Array as PropType<MenuItem[]>,
      required: true,
    },
  },
  emits: ['update:modelValue'],
  setup(props, { emit }) {
    const drawerOpen = computed({
      get: () => props.modelValue,
      set: (val: boolean) => emit('update:modelValue', val),
    });

    return {
      drawerOpen,
    };
  },
});
</script>
