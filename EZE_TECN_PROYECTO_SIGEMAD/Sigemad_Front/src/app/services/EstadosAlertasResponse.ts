import { EstadoAlerta } from "../models/estadoAlerta";

export interface EstadosAlertasResponse {
    count: number,
    data: EstadoAlerta[],
    pageCount: number,
    pageIndex: number,
    pageSize: number,
  }