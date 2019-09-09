import styled from 'styled-components'
import {
  border,
  color,
  flexbox,
  layout,
  space,
  BorderProps,
  ColorProps,
  FlexboxProps,
  LayoutProps,
  SpaceProps,
} from 'styled-system'

// eslint-disable-next-line
const Box = styled.div<
  BorderProps & ColorProps & FlexboxProps & LayoutProps & SpaceProps
>`
  display: flex;
  box-sizing: border-box;

  ${border}
  ${color}
  ${flexbox}
  ${layout}
  ${space}
`

export default Box
