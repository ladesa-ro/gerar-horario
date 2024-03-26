import { parse } from "date-fns";

export const parseTime = (time: string) => parse(time, "HH:mm", new Date());
