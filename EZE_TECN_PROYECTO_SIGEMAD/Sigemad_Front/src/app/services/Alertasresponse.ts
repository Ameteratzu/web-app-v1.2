import { Alerta } from "../models/alerta";

export interface AlertasResponse {
    count: number,
    data: Alerta[],
    pageCount: number,
    pageIndex: number,
    pageSize: number,
  }