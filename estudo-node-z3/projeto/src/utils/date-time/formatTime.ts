import { format } from "date-fns";

export const formatTime = (time: Date | number | string) =>
  format(time, "HH:mm");
